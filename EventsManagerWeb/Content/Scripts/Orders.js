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

function GetOrders() {
    const token = (document.cookie).split("=")[1];
    console.log(token)

    if (!token) {
        console.error('Bearer token not found in the "Token" cookie.');
        return;
    }

    const headers = {
        'Authorization': `Bearer ${token}`
    };
    fetch('/api/GetOrders', { headers })
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
function LoadOrders() {
    $("#MenusWrapper").empty();
    let ResultCouner = 0;
    const SearchVal = document.getElementById("MenuSearch").value ?? '';

    MenuResponse.Orders.forEach(menu => {
        if (
            isSearched(SearchVal, menu) &&
            isChecked(".HallCheckBox", menu.HallId)
        ) {
            $("#MenusWrapper").append(
                '<div id="MenuItem">' +
                    '<a href="/Menu/?id=' + menu.MenuId + '">' + menu.MenuName + '</a>' +
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