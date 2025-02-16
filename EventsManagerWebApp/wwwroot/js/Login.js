$(document).ready(function () {
    // Validate Username 
    $("#UserName-Check").hide();
    $("#UserName").keyup(function () {
        validateUsername();
    });

    // Validate Password 
    $("#Password-Check").hide();
    $("#Password").keyup(function () {
        validatePassword();
    });
});

function validateUsername() {
    let usernameValue = $("#UserName").val();
    if (usernameValue == null) {
        $("#UserName-Check").html("**UserName can't be empty");
        $("#UserName-Check").show();
        return false;
    } else {
        $("#UserName-Check").hide();
    }
    return true;
}

function validatePassword() {
    let passwordValue = $("#Password").val();
    if (passwordValue == null) {
        $("#Password-Check").html("**Password can't be empty");
        $("#Password-Check").show();
        return false;
    } else {
        $("#Password-Check").hide();
    }
    return true;
}

function validateLogin() {
    if (validateUsername() && validatePassword()) {
        return true;
    } else {
        return false;
    }
}