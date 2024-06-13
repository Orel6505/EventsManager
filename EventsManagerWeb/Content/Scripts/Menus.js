var MenuResponse;

function ShowOptions(elements) {
    const ex = document.getElementById(elements);
    ex.style.maxHeight = (ex.style.maxHeight === "0px") ? "600px" : "0px";
}
function ShowHallsOptions() {
    ShowOptions("HallOptions");
}

function ShowFoodTypeOptions() {
    ShowOptions("FoodTypeOptions");
}

function ShowFoodOptions() {
    ShowOptions("FoodOptions");
}

async function GetMenus() {
    await fetch('/api/GetMenus')
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error fetching menus: ${response.status} ${response.statusText}`);
            }
            return response.json();
        })
        .then(data => {
            globalThis.MenuResponse = data;
            LoadMenus();
        })
        .catch(error => {
            console.error('Error fetching menus:', error);
        });
}
function LoadMenus() {
    $("#MenusWrapper").empty();
    let ResultCouner = 0;
    const SearchVal = document.getElementById("MenuSearch").value ?? '';

    MenuResponse.Menus.forEach(menu => {
        if (
            isSearched(SearchVal, menu) &&
            isChecked(".HallCheckBox", menu.HallId) &&
            isCheckedLong(".ShowNoFood", menu.FoodIds) &&
            isCheckedArray(".FoodCheckBox", menu.FoodIds) &&
            isCheckedLong(".FoodTypeCheckBox", menu.FoodTypeIds) &&
            isCheckedArray(".FoodTypeCheckBox", menu.FoodTypeIds)
        ) {
            $("#MenusWrapper").append(
                '<div class="product" data-category="'+ menu.HallId + '" data-price="699">' +
                    '<img src="'+ menu.MenuId + '.png" alt="תמונת תפריט">' +
                    '<h3>' + menu.MenuName + '</h3>' +
                    '<p>Price: $699</p>' +
                    '<button type="button" class="SortMenuOptions learnmore" onclick="window.location.href=\'/Menu/?id=' + menu.MenuId +'\';">למד עוד</button>' +
                '</div>'
            );
            ResultCouner++;
        }
    });
    if (!ResultCouner && SearchVal) {
        $("#MenusWrapper").append('<div id="MenuItem">' + 'לא נמצאו תפריטים עם השם הזה.' + '</div>');
    }
}

function isSearched(SearchVal, menu) {
    if (SearchVal !== null) { // strict not-equal operator
        if (menu.MenuName.includes(SearchVal)) {
            return true;
        } else {
            return false;
        }
    }
    return true;
}

function isChecked(element, num) {
    const checkboxes = document.querySelectorAll(element);
    let isTouched = true;
    for (const checkbox of checkboxes) {
        if (checkbox.checked) {
            isTouched = false;
            if (checkbox.value == num) {
                return true;
            }
        }
    }
    return isTouched;
}

function isCheckedArray(element, arr) {
    const checkboxes = document.querySelectorAll(element);
    let isTouched = true;
    for (const checkbox of checkboxes) {
        if (checkbox.checked) {
            isTouched = false;
            if (arr.includes(parseInt(checkbox.value))) {
                isTouched = true;
            }
        }
    }
    return isTouched;
}

function isCheckedLong(element, arr) {
    const checkboxes = document.querySelectorAll(element);
    let isTouched = false;
    for (const checkbox of checkboxes) {
        if (checkbox.checked) {
            if (arr.length > 0) {
                isTouched = true;
            }
        } else {
            isTouched = true;
        }
    }
    return isTouched;
}