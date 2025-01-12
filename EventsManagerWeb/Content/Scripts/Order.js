const daysTag = document.querySelector(".days");
const currentDate = document.querySelector(".current-date");
const prevNextIcon = document.querySelectorAll(".icons span");

let date = new Date();
let currYear = date.getFullYear();
let currMonth = date.getMonth();

const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

window.addEventListener("load", () => {
    $("#OrderForm").hide();
});

const renderCalendar = () => {
    let firstDayofMonth = new Date(currYear, currMonth, 1).getDay(), 
        lastDateofMonth = new Date(currYear, currMonth + 1, 0).getDate(),
        lastDayofMonth = new Date(currYear, currMonth, lastDateofMonth).getDay(), 
        lastDateofLastMonth = new Date(currYear, currMonth, 0).getDate();
    let liTag = "";

    for (let i = firstDayofMonth; i > 0; i--) {
        liTag += `<li class="inactive">${lastDateofLastMonth - i + 1}</li>`;
    }

    for (let i = 1; i <= lastDateofMonth; i++) {
        let isToday = "";
        if (currMonth === new Date().getMonth() && currYear === new Date().getFullYear()) {
            isToday = i < date.getDate() ? "inactive" : "";
            isToday = i === date.getDate() ? "active" : isToday;
        }
        liTag += `<li class="${isToday}"><button class="check-availability" onclick="" data-date="${currYear}-${currMonth + 1}-${i}" value="${i}/${currMonth + 1}/${currYear}">${i}</button></li>`;
    }

    //for (let i = lastDayofMonth; i < 6; i++) {
    //    liTag += `<li class="inactive">${i - lastDayofMonth + 1}</li>`
    //}

    currentDate.innerText = `${months[currMonth]} ${currYear}`;
    daysTag.innerHTML = liTag;
}
renderCalendar();

prevNextIcon.forEach(icon => {
    icon.addEventListener("click", () => {
        let newMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;
        if (newMonth >= new Date().getMonth() && currYear === new Date().getFullYear() || currYear > new Date().getFullYear()) {
            currMonth = newMonth;
            if (currMonth < 0 || currMonth > 11) {
                date = new Date(currYear, currMonth, new Date().getDate());
                currYear = date.getFullYear();
                currMonth = date.getMonth();
            } else {
                date = new Date();
            }
            renderCalendar();
        }
    });
});

$("button").click(function () {
    let buttonval = $(this).val();
    let HallId = $('#HallId').val();
    checkHallAvailability(buttonval, HallId);
});

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

async function checkHallAvailability(dateval, HallId) {
    $("#EventType").empty();
    $("#Hall-Response").empty();
    $("#OrderForm").hide();
    $("#Order").empty();
    $("#Order").append(
        '<input type="hidden" id="EventDate" name="EventDate" value="' + escapeHtml(dateval) +'" />'
    );
    try {
        const response = await fetch(`https://localhost:44365/api/HallAvailability?EventDate=${dateval}&HallId=${HallId}`);
        if (!response.ok) {
            $("#Hall-Response").show();
            $("#Hall-Response").html("**Error fetching username availability. Please try again later.");
            return false;
        }
        const data = await response.json(); // Parse the JSON data
        data.forEach((event) => {
            $("#EventTypeId").append(
                '<option value=' + event.EventTypeId + '>' + event.EventTypeName + '</option>'
            );
        });
        $("#OrderForm").show();
    } catch (error) {
        console.error('Error fetching data:', error);
        $("#Hall-Response").show();
        $("#Hall-Response").html("**Error fetching username availability. Please try again later.");
        return false;
    }
}