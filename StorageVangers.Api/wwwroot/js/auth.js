var authenticatedUser;

function SignIn(user) {
    authenticatedUser = user;
    var userNameElem = document.querySelector(".signedin-user-name");
    if (userNameElem) {
        userNameElem.textContent = authenticatedUser.displayName;
    }
    var userImgElem = document.querySelector(".signedin-user-img");
    if (userImgElem) {
        userImgElem.setAttribute("src", authenticatedUser.photoURL);
    }

    if (window.location.pathname === "/" || window.location.pathname === "/index" || window.location.pathname === "/index.html") {
        window.location.replace("/app");
    }
}

function SignOut() {
    authenticatedUser = null;
    localStorage.removeItem("googleAccessToken");
    localStorage.removeItem("googleRefreshToken");
    var userNameElem = document.querySelector(".signedin-user-name");
    if (userNameElem) {
        userNameElem.textContent = "";
    }
    var userImgElem = document.querySelector(".signedin-user-img");
    if (userImgElem) {
        userImgElem.removeAttribute("src");
    }
}

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

window.addEventListener("DOMContentLoaded", (event) => {
    firebase.auth().onAuthStateChanged(function (user) {
        if (user) {
            SignIn(user);
        }
        else {
            SignOut();
        }
    });

    firebase.auth().getRedirectResult()
        .then(function (result) {
            authenticatedUser = result.user;
            if (result.credential.providerId == "google.com") {
                localStorage.setItem("googleAccessToken", result.credential.accessToken);
                localStorage.setItem("googleRefreshToken", authenticatedUser.refreshToken);
            }
            else if (user.providerId == "microsoft.com") {
                localStorage.setItem("microsoftAccessToken", result.credential.accessToken);
                localStorage.setItem("microsoftRefreshToken", authenticatedUser.refreshToken);
            }
        })
        .catch(function (error) {
            console.log(error);
        });
});

firebase.auth().useDeviceLanguage();

var googleProvider = new firebase.auth.GoogleAuthProvider();
googleProvider.addScope("https://www.googleapis.com/auth/drive");
googleProvider.addScope("https://www.googleapis.com/auth/userinfo.profile");

var microsoftProvider = new firebase.auth.OAuthProvider("microsoft.com");
microsoftProvider.addScope("email");
microsoftProvider.addScope("Files.ReadWrite.All");
microsoftProvider.addScope("offline_access");
microsoftProvider.addScope("openid");
microsoftProvider.addScope("profile");
microsoftProvider.addScope("User.Read");
microsoftProvider.addScope("User.ReadBasic.All");

var btnMSSignIn = document.querySelector(".ms-signin");
if (btnMSSignIn) {
    btnMSSignIn.addEventListener("click", function () {
        firebase.auth().signInWithRedirect(microsoftProvider);
        //firebase.auth().signInWithPopup(microsoftProvider)
        //    .then(function (result) {
        //        var idpData = result.additionalUserInfo.profile;
        //        var microsoftAccessToken = result.credential.accessToken;
        //        var microsoftOauthIdToken = result.credential.idToken;
        //    })
        //    .catch(function (error) {
        //        var err = error;
        //    });
    });
}

var btnGSignIn = document.querySelector(".google-signin");
if (btnGSignIn) {
    btnGSignIn.addEventListener("click", function () {
        firebase.auth().signInWithRedirect(googleProvider);
    });



}

function updateOptions(options) {
    const update = { ...options };

    let customHeaders = {};
    if (localStorage.getItem("googleAccessToken") != null && localStorage.getItem("googleRefreshToken") != null) {
        customHeaders = {
            GoogleAccessToken: localStorage.getItem("googleAccessToken"),
            GoogleRefreshToken: localStorage.getItem("googleRefreshToken")
        };
    }

    update.headers = {
        ...update.headers,
        ...customHeaders
    };

    return update;
}

function fetcher(url, options) {
    return fetch(url, updateOptions(options));
}

var btnSignOut = document.querySelector(".signout");
if (btnSignOut) {
    btnSignOut.addEventListener("click", function () {
        firebase.auth().signOut()
            .then(
                function () {
                    SignOut();
                    window.location.replace("/");
                },
                function (error) {
                    console.log(error);
                }
            );
    });
}