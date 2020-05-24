# StorageVangers

This is just an initial version of the final project. Our aim is to create a single source of truth for all your files across different 3rd party storage providers (like Google Drive, Microsoft OneDrive, DropBox, etc.), cloud storage services (like S3, Azure Blob Storage, Azure File Storage, Google Cloud Storage, etc.) and your own file servers (we are planning to support different protocols such as FTP, SFTP, etc.).

The main feature of the project will be to sort all your files and folders automatically (upon approval ofcourse) across all your storage services (we're planning to incorporate ML to help with this task).

#### Current set of features:

* [x] Add Firebase Auth
* [x] Implement Google Drive
* [ ] Implement Upload Files and Folder Creation
* [ ] Implement Search
* [ ] Implement auto-sorting of uploaded files
* [ ] Implement Microsoft OneDrive
* [ ] Implement other providers
* [ ] Implement Plugin System

> NOTE: Right now the project just emulates Google Drive Api v3 due to short time of this hackathon but we will for sure add other features very soon. Stay tuned.
