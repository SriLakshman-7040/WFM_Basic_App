

$(function () {
    $("#loginsubmitBtn").on('click', userDetailValidate);
})

function userDetailValidate() {

    let name = document.getElementById("username").value;
    let password = document.getElementById("secret").value;

    const userCredential = {
        userName: name,
        userPassword: password
    }
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(userCredential)
    };

    fetch('/User/LogInPage' + requestOptions)
        .then(response => response.text())
        .then(response => {
            console.log(response);
            if (response == "OK") {
                alert('Login Successfully..! - User Type Manager');
                window.location.replace("https://localhost:7283/Manager/ManagerLandingPage");
            }
            else if (response == "Accepted"){                
                alert('Login Successfully..! - User Type Member');
                window.location.replace("https://localhost:7283/MemberMvc/MemberLandingPage");
            }
        })
        .catch(err => {
            console.log(err);
        })
}