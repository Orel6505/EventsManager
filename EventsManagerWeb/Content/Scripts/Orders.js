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
    fetch('/api/GetOrders')
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error fetching menus: ${response.status} ${response.statusText}`);
            }
            return response.json();
        })
        .then(data => {
            globalThis.MenuResponse = data;
            LoadOrders();
        })
        .catch(error => {
            console.error('Error fetching menus:', error);
        });
}
function LoadOrders() {
    $("#Orders").empty();
    let ResultCouner = 0;
    const SearchVal = document.getElementById("MenuSearch").value ?? '';

    MenuResponse.Orders.forEach(Order => {
        if (
            isSearched(SearchVal, Order)
        ) {
            let OrderDate = new Date(Order.OrderDate * 1000);
            let OrderDateString = OrderDate.toLocaleString();
            $("#Orders").append(
                '<tr>' +
                    '<td>' + "Order-" + Order.OrderId + '</td>' +
                    '<td>' + OrderDateString + '</td>' +
                    '<td>' + Order.EventType.EventTypeName + " " + Order.EventDate + '</td>' +
                    '<td>' + '<a href="/Menu/?id=' + Order.MenuId + '">' + Order.menu.MenuName + '</a>' + '</td>' +
                    '<td>' + Order.NumOfPeople + '</td>' +
                    '<td>' + Order.IsPaid + '</td>' +
                '</tr>'
            );
            ResultCouner++;
        }
    });
    if (!ResultCouner && SearchVal) {
        $("#Orders").append('<div>' + 'לא נמצאו תפריטים עם השם הזה.' + '</div>');
    }
}

function isSearched(SearchVal, Order) {
    if (SearchVal !== null) { // strict not-equal operator
        if (Order.EventType.EventTypeName.includes(SearchVal)) {
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