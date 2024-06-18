$(document).ready(function () {
    // Validate Username 
    $("#PasswordConfirm-Check").hide();
    $("#PasswordConfirm").keyup(function () {
        validateConfirmPassword();
    });

    // Validate Password 
    $("#Password-Check").hide();
    $("#NewPassword").keyup(function () {
        validatePassword();
    });
});

function validatePassword() {
    let passwordValue = $("#NewPassword").val();
    if (passwordValue.length == "") {
        $("#Password-Check").hide();
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
            $("#Password-Check").show();
            $("#Password-Check").html(
                "**Password must contain uppercase, lowercase, numbers, symbols, and be at least " + minimumLength + " characters long"
            );
            return false;
        }
        $("#Password-Check").hide();
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

function validateLogin() {
    if (validateConfirmPassword() && validatePassword()) {
        return true;
    } else {
        return false;
    }
}