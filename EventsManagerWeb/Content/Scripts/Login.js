// Document is ready 
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

    // Validate Password 
    $("#Password-Check").hide();
    $("#Password").keyup(function () {
        validatePassword();
    });

    $("#PhoneNum-Check").hide();
    $("#PhoneNum").keyup(function () {
        validatePhoneNum();
    });

    // Validate Confirm Password 
    $("#PasswordConfirm-Check").hide();
    $("#PasswordConfirm").keyup(function () {
        validateConfirmPassword();
    });
});

function validateUsername() {
    let usernameValue = $("#UserName").val();
    if (!usernameValue.length) {
        $("#UserName-Check").hide();
        return false;
    } else if (usernameValue.length < 3 || usernameValue.length > 10) {
        $("#UserName-Check").show();
        $("#UserName-Check").html("**length of username must be between 3 and 10");
        return false;
    } else {
        $("#UserName-Check").hide();
    }
    return true;
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
function validatePassword() {
    let passwordValue = $("#Password").val();
    if (passwordValue.length == "") {
        $("#Password-Check").show();
        return false;
    } else {
        $("#Password-Check").hide();
    }
    return true;
}

function validateConfirmPassword() {
    let confirmPasswordValue = $("#PasswordConfirm").val();
    let passwordValue = $("#Password").val();
    if (passwordValue != confirmPasswordValue) {
        $("#PasswordConfirm-Check").show();
        $("#PasswordConfirm-Check").html("**Password didn't Match");
        $("#PasswordConfirm-Check").css("color", "red");
        return false;
    } else {
        $("#PasswordConfirm-Check").hide();
    }
    return true;
}

function validateRegister() {
    if (validateUsername() && validatePassword() && validateConfirmPassword() && validateEmail() && validatePhone()) {
        return true;
    } else {
        return false;
    }
}

function validateLogin() {
    if (validateUsername() && validatePassword()) {
        return true;
    } else {
        return false;
    }
}