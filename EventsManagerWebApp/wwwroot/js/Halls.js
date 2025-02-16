var HallResponse;

async function GetHalls() {
    await fetch('/api/GetHalls')
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error fetching Halls: ${response.status} ${response.statusText}`);
            }
            return response.json();
        })
        .then(data => {
            globalThis.HallResponse = data;
            LoadHalls();
        })
        .catch(error => {
            console.error('Error fetching Halls:', error);
        });
}
function LoadHalls() {
    $("#HallsWrapper").empty();
    let ResultCounter = 0;

    HallResponse.forEach(Hall => {
        $("#HallsWrapper").append(
            '<div class="product" data-category="'+ Hall.HallId + '">' +
                '<img src="'+ Hall.HallImage + '" alt="תמונת ' + '">' +
                '<h3>' + Hall.HallName + '</h3>' +
                '<p>' + Hall.HallDesc + '</p>' +
                '<p>מספר אנשים מרבי: ' + Hall.MaxPeople + '</p>' +
                '<button type="button" class="SortHallOptions learnmore" onclick="window.location.href=\'/Hall/?id=' + Hall.HallId +'\';">למד עוד</button>' +
            '</div>'
        );
        ResultCounter++;
    });

    if (!ResultCounter && SearchVal) {
        $("#HallsWrapper").append('<div id="HallItem">לא נמצאו אולמות עם השם הזה.</div>');
    }
}