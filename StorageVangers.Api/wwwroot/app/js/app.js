function SetDriveItemCollection(data) {
    let folders = data.filter(f => f.mimeType === "application/vnd.google-apps.folder");
    let files = data.filter(f => f.mimeType !== "application/vnd.google-apps.folder");
    SetDriveItemCollectionForFolder(folders);
    SetDriveItemCollectionForFiles(files);
    
    let driveItems = document.querySelectorAll(".driveItem");
    for (let idx = 0; idx < driveItems.length; idx++) {
        driveItems[idx].addEventListener("click", driveItemClick);
    }
}

function SetDriveItemCollectionForFolder(folders) {
    document.querySelector(".folders-fieldset").classList.remove("displayNone");
    var driveItemCollectionFolders = document.querySelector(".driveItemCollection.folders");
    for (var i = 0; i < folders.length; i++) {
        driveItemCollectionFolders.innerHTML += `
            <div class="driveItem m-1" data-id="${folders[i].id}" data-folder="false">
                <img src="${folders[i].iconLink}" />
                <span>${folders[i].name}</span>
            </div>
        `;
    }

    if (folders.length <= 0) {
        document.querySelector(".folders-fieldset").classList.add("displayNone");
    }
}

function SetDriveItemCollectionForFiles(files) {
    document.querySelector(".files-fieldset").classList.remove("displayNone");
    var driveItemCollectionFiles = document.querySelector(".driveItemCollection.files");
    driveItemCollectionFiles.removeAttribute("style");
    for (var i = 0; i < files.length; i++) {
        driveItemCollectionFiles.innerHTML += `
            <div class="driveItem m-1" data-id="${files[i].id}" data-folder="false">
                <img src="${files[i].iconLink}" />
                <span>${files[i].name}</span>
            </div>
        `;
    }

    if (files.length <= 0) {
        document.querySelector(".files-fieldset").classList.add("displayNone");
    }
}

function driveItemClick(e) {
    let id = e.target.dataset.id;
    let isFolder = e.target.dataset.folder;

    if (isFolder) {
        fetcher(`/api/storage/GetFilesBydId/${id}`)
            .then(resp => resp.json())
            .then(data => {
                SetDriveItemCollection(data);
            })
            .catch(err => {
                console.log(err);
            });

        let driveItemCollections = document.querySelectorAll(".driveItemCollection");
        for (let idx = 0; idx < driveItemCollections.length; idx++) {
            driveItemCollections[idx].innerHTML = "";
        }
    }
}

document.addEventListener('DOMContentLoaded', (e) => {
    fetcher("/api/storage/GetFiles")
        .then(resp => resp.json())
        .then(data => {
            SetDriveItemCollection(data);
        })
        .catch(err => {
            console.log(err);
        });
});