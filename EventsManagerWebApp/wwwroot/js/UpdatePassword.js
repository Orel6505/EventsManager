$(document).ready(function () {
    // Validate Username 
    $("#PasswordConfirm-Check").hide();
    $("#PasswordConfirm").keyup(function () {
        validateConfirmPassword();
    });

    // Validate Password 
    $("#NewPassword-Check").hide();
    $("#NewPassword").keyup(function () {
        validateNewPassword();
        validateConfirmPassword();
    });
    $("#Password-Check").hide();
    $("Password").keyup(function () {
        validatePassword();
    });
});

async function checkPassword() {
    const input = $("#Password");
    try {
        const response = await fetch("/api/CheckPassword?Password=" + input.val());
        if (!response.ok) {
            $("#Password-Check").show();
            $("#Password-Check").html("**Error checking Password, Please try again later");
            return false;
        }
        let result = await response.json();
        if (!result) {
            $("#Password-Check").show();
            $("#Password-Check").html("**Error checking Password, Please try again later");
        }
        return result;
    } catch (error) {
        $("#Password-Check").show();
        $("#Password-Check").html("**Error checking Password, Please try again later");
        return false;
    }
}

function validateNewPassword() {
    let passwordValue = $("#NewPassword").val();
    if (passwordValue.length == "") {
        $("#NewPassword-Check").hide();
        return false;
    } else {
        const hasUppercase = /[A-Z]/.test(passwordValue);
        const hasLowercase = /[a-z]/.test(passwordValue);
        const hasNumber = /\d/.test(passwordValue);
        const hasSymbol = /[^a-zA-Z0-9]/.test(passwordValue);
        const minimumLength = 8;

        if (
            !hasUppercase ||
            !hasLowercase ||
            !hasNumber ||
            !hasSymbol ||
            passwordValue.length < minimumLength
        ) {
            $("#NewPassword-Check").show();
            $("#NewPassword-Check").html(
                "**Password must contain uppercase, lowercase, numbers, symbols, and be at least " + minimumLength + " characters long"
            );
            return false;
        }
        $("#NewPassword-Check").hide();
    }
    return true;
}
function validateConfirmPassword() {
    let confirmPasswordValue = $("#PasswordConfirm").val();
    let passwordValue = $("#NewPassword").val();
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

function validatePassword() {
    if (validateConfirmPassword() && validateNewPassword() && checkPassword()) {
        return true;
    } else {
        return false;
    }
}