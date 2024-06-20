var MenuResponse;

async function GetOrders() {
    await fetch('/api/GetOrders')
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
    MenuResponse.Orders.forEach(Order => {
        let OrderDate = new Date(Order.OrderDate * 1000);
        let OrderDateString = OrderDate.toLocaleString();
        $("#Orders").append(
            '<tr>' +
                '<td>' + "Order-" + Order.OrderId + '</td>' +
                '<td>' + OrderDateString + '</td>' +
                '<td class="hiddenonlow">' + Order.EventType.EventTypeName + " " + Order.EventDate + '</td>' +
                '<td class="hiddenonlow">' + '<a href="/Menu/?id=' + Order.HallId + '">' + Order.HallId + '</a>' + '</td>' +
                '<td class="hiddenonlow">' + '<a href="/Menu/?id=' + Order.MenuId + '">' + Order.ChosenMenu.MenuName + '</a>' + '</td>' +
                '<td class="hiddenonlow">' + Order.NumOfPeople + '</td>' +
                '<td>' + Order.IsPaid + '</td>' +
                '<td class="shownonlow"><button class="show-more-btn">Details</button></td>' +
            '</tr>'
        );
        ResultCouner++;
    });
    if (!ResultCouner) {
        $("#Orders").append('<div>' + 'לא נמצאו תפריטים עם השם הזה.' + '</div>');
    }
}