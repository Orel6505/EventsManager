var MenuResponse;

function ShowOptions(elements) {
    const ex = document.getElementById(elements);
    ex.style.maxHeight = ex.style.maxHeight === "600px" ? "0" : "600px";
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

function GetMenus() {
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        try {
            globalThis.MenuResponse = JSON.parse(this.responseText);
            LoadMenus();
        } catch (error) {
            console.error("Error fetching menus:", error);
        }
    };
    xhttp.onerror = function () {
        console.error("Error during XHR request for menus.");
    };
    xhttp.open("GET", "/Data/GetMenus", true);
    xhttp.send();
}
function LoadMenus() {
    $("#MenusWrapper").empty();
    let ResultCouner = 0;
    let SearchVal = document.getElementById("MenuSearch").value ?? '';
    MenuResponse.Menus.forEach((menu) => {
        if (!isSearched(SearchVal, menu)) {
            //pass
        } else if (!isChecked(".HallCheckBox", menu.HallId)) {
            //pass
        } else if (!isCheckedLong(".ShowNoFood", menu.FoodIds)) {
            //pass
        } else if (!isCheckedArray(".FoodCheckBox", menu.FoodIds)) {
            //pass
        } else {
            $("#MenusWrapper").append('<div id="MenuItem">' +
                '<a href="/ViewMenu/?id=' + menu.MenuId + '">' + menu.MenuName + '</a>' +
                '</div>');
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
                return true;
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