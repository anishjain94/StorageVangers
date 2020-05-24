var firebaseConfig = {
    apiKey: "AIzaSyBtSHJMh_TAKJEdmLbjRy-mYY19emKF9Lk",
    authDomain: "storagevangers-w-1590259768184.firebaseapp.com",
    databaseURL: "https://storagevangers-w-1590259768184.firebaseio.com",
    projectId: "storagevangers-w-1590259768184",
    storageBucket: "storagevangers-w-1590259768184.appspot.com",
    messagingSenderId: "190848468083",
    appId: "1:190848468083:web:2ef0ac4721a7e11fa85606",
    measurementId: "G-NHLZ3FDY9D",
};

firebase.initializeApp(firebaseConfig);
var user;

window.addEventListener("DOMContentLoaded", (event) => {
    firebase.auth().onAuthStateChanged(function (user) {

        if (user) {
            user = user;
        }

        console.log(user);
        // console.log(user.credential.accessToken);
    });

    firebase
        .auth()
        .getRedirectResult()
        .then(function (result) {
            console.log(result);
            user = result.user;
            if (user.providerId == "google.com") {
                localStorage.googleaccessToken = user.credential.accessToken;
                localStorage.googlerefreshToken = user.refreshToken;
            }
            else if (user.providerId == "microsoft.com") {
                localStorage.microsoftaccessToken = user.credential.accessToken;
                localStorage.microsoftrefreshToken = user.refreshToken;
            }
        })
        .catch(function (error) {
            console.log(error);
        });

    console.log(firebase.auth.UserCredential);
});

firebase.auth().useDeviceLanguage();

var googleProvider = new firebase.auth.GoogleAuthProvider();
googleProvider.addScope("https://www.googleapis.com/auth/drive");
googleProvider.addScope(
    "https://www.googleapis.com/auth/userinfo.profile"
);

var microsoftProvider = new firebase.auth.OAuthProvider("microsoft.com");
microsoftProvider.addScope("email");
microsoftProvider.addScope("Files.ReadWrite.All");
microsoftProvider.addScope("offline_access");
microsoftProvider.addScope("openid");
microsoftProvider.addScope("profile");
microsoftProvider.addScope("User.Read");
microsoftProvider.addScope("User.ReadBasic.All");

document
    .querySelector("#ms-signin")
    .addEventListener("click", function () {
        firebase
            .auth()
            .signInWithPopup(microsoftProvider)
            .then(function (result) {
                var idpData = result.additionalUserInfo.profile;
                var microsoftAccessToken = result.credential.accessToken;
                var microsoftOauthIdToken = result.credential.idToken;
            })
            .catch(function (error) {
                var err = error;
            });
    });

document
    .querySelector("#google-signin")
    .addEventListener("click", function () {
        firebase.auth().signInWithRedirect(googleProvider);
        //firebase.auth().signInWithPopup(googleProvider)
        //    .then(function (result) {
        //        var googleAccessToken = result.credential.accessToken;
        //        var googleUser = result.user;
        //    })
        //    .catch(function (error) {
        //        var errorCode = error.code;
        //        var errorMessage = error.message;
        //        var email = error.email;
        //        var credential = error.credential;
        //    });
    });

function updateOptions(options) {
    const update = { ...options };

    update.headers = {
        ...update.headers,
        googleaccesstoken: localStorage.googleaccessToken,
        googlerefreshtoken: localStorage.googlerefreshToken,

        microsoftaccesstoken: localStorage.microsoftaccessToken,
        microsoftfreshtoken: localStorage.microsoftrefreshToken

    };
    return update;
}

function fetcher(url, options) {
    return fetch(url, updateOptions(options));
}


// const myHeaders = new Headers();
// myHeaders.append({});
// const myRequest = new Request("/getuserinfo", {
//     method: "GET",
//     headers: myHeaders,
//     mode: "cors",
//     cache: "default",
// });

// fetch(myRequest)
//     .then((response) => response.blob())
//     .then((jsn) => {
//         (document.getElementById("name").innerHTML = jsn.userName),
//             (document.getElementById("profile").src = jsn.avatarUrl);
//     });

document
    .querySelector("#ms-signout")
    .addEventListener("click", function () {
        firebase
            .auth()
            .signOut()
            .then(
                function () {
                    console.log("Signed Out");
                },
                function (error) {
                    var err = error;
                    console.error("Sign Out Error", error);
                }
            );
    });