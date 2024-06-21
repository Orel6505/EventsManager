$(document).ready(function () {
    // Validate Username 
    $("#UserName-Check").hide();
    $("#UserName").keyup(function () {
        validateUsername();
    });

    // Validate Email 
    $("#Email-Check").hide();
    $("#Email").keyup(function () {
        validateEmail();
    });

    $("#PhoneNum-Check").hide();
    $("#PhoneNum").keyup(function () {
        validatePhoneNum();
    });
});

async function validateUsername() {
    let usernameValue = $("#UserName").val();
    if (!usernameValue.length) { // check if user is empty
        $("#UserName-Check").hide();
        return false;
    }
    if (usernameValue.length < 3 || usernameValue.length > 12) { //check length of value
        $("#UserName-Check").show();
        $("#UserName-Check").html("**length of username must be between 3 and 12");
        return false;
    }
    if (usernameValue != document.getElementById("UserName").getAttribute("value"))
    {
        if (await checkUserNameAvailability(usernameValue)) { //check if username is taken
            $("#UserName-Check").show();
            $("#UserName-Check").html("**UserName Is Already Taken");
            return false;
        } 
    }
    $("#UserName-Check").hide();
    return true;
}

async function checkUserNameAvailability(usernameValue) {
    try {
        const response = await fetch("/api/IsAvailableUserName?UserName=" + usernameValue);
        if (!response.ok) {
            $("#UserName-Check").show();
            $("#UserName-Check").html("**Error fetching username availability, Please try again later");
            return false;
        }
        return await response.text() === "false";
    } catch (error) {
        $("#UserName-Check").show();
        $("#UserName-Check").html("**Error fetching username availability, Please try again later");
        return false;
    }
}

function validateEmail() {
    const emailValue = $("#Email").val();
    const emailRegex = /^([_\-\.0-9a-zA-Z]+)@([_\-\.0-9a-zA-Z]+)\.([a-zA-Z]){2,7}$/;
    if (emailRegex.test(emailValue)) {
        $("#Email-Check").hide();
        return true;
    } else {
        $("#Email-Check").show();
        return false;
    }
}

function validatePhoneNum() {
    let phoneValue = $("#PhoneNum").val();
    const phoneRegex = /^0(5[1-9])[0-9]{7}$/;
    if (!phoneRegex.test(phoneValue)) {
        $("#PhoneNum-Check").show();
        return false;
    } else {
        $("#PhoneNum-Check").hide();
    }
    return true;
}
function validateUpdate() {
    if (validateUsername() && validateEmail() && validatePhone()) {
        return true;
    } else {
        return false;
    }
}