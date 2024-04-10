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
        if (SearchVal !== null) { // strict not-equal operator
            if (menu.MenuName.includes(SearchVal)) {
                $("#MenusWrapper").append('<div id="MenuItem">' +
                    '<a href="/ViewMenu/?id=' + menu.MenuId + '">' + menu.MenuName + '</a>' +
                    '</div>');
                ResultCouner++;
            }
        } else {
            $("#MenusWrapper").append('<div id="MenuItem">' +
                '<a href="/ViewMenu/?id=' + menu.MenuId + '">' + menu.MenuName + '</a>' +
                '</div>');
        }
    });
    if (!ResultCouner && SearchVal) {
        $("#MenusWrapper").append('<div id="MenuItem">' + 'לא נמצאו תפריטים עם השם הזה.' + '</div>');
    }
}

function binarySearch(arr, num) {
    let m = 0;
    let n = arr.length - 1;
    while (m <= n) {
        let k = (n + m) >> 1;
        let cmp = num - arr[k];
        if (cmp > 0) {
            m = k + 1;
        } else if (cmp < 0) {
            n = k - 1;
        } else {
            return k;
        }
    }
    return ~m;
}