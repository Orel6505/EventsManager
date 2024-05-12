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

async function validateUsername() {
    let usernameValue = $("#UserName").val();
    if (!usernameValue.length) {
        $("#UserName-Check").hide();
        return false;
    } else if (usernameValue.length < 3 || usernameValue.length > 12) {
        $("#UserName-Check").show();
        $("#UserName-Check").html("**length of username must be between 3 and 12");
        return false;
    } else if (await checkUserNameAvailability(usernameValue)) {
        $("#UserName-Check").show();
        $("#UserName-Check").html("**UserName Is Already Taken");
        return false;
    } else {
        $("#UserName-Check").hide();
    }
    return true;
}

async function checkUserNameAvailability(usernameValue) {
    try {
        const response = await fetch("/api/IsAvailableUserName?UserName=" + usernameValue);
        if (!response.ok) {
            return false;
            $("#UserName-Check").show();
            $("#UserName-Check").html("**Error fetching username availability, Please try again later");
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
function validatePassword() {
    let passwordValue = $("#Password").val();
    if (passwordValue.length == "") {
        $("#Password-Check").show();
        return false;
    } else {
        const hasUppercase = /[A-Z]/.test(passwordValue);
        const hasLowercase = /[a-z]/.test(passwordValue);
        const hasNumber = /[0-9]/.test(passwordValue);
        const hasSymbol = /[^a-zA-Z0-9]/.test(passwordValue);
        const minimumLength = 8;

        if (
            !hasUppercase ||
            !hasLowercase ||
            !hasNumber ||
            !hasSymbol ||
            passwordValue.length < minimumLength
        ) {
            $("#Password-Check").show();
            $("#Password-Check").html(
                "**Password must contain uppercase, lowercase, numbers, symbols, and be at least " + minimumLength +" characters long"
            );
            return false;
        }
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
    return true
}