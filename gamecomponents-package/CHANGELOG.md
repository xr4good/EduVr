# 1.0.0

2022-09-02

### Featuring changes

* Created package


# 1.0.1

2022-09-29

### Featuring changes

* SaveStatement method in BaseStatementSender.cs is now async, meaning that the game will not stutter anymore when waiting for the LRS response

### Bug Fixes

* Adjusted some inconsistencies in the Components prefabs
* Fixed a bug where the QuizController woudn't work if initialized with zero Questions in the registry, even if the questions were added in the future
* Fixed a bug where a NullReferenceException was being thrown at SlideController.cs in the Start method


# 1.1.0

2022-10-09

This update mainly focused in developing a way to communicate with a pre-designed LMS and facilitate the LRS comunication by making the deployment a bit simpler.

### Featuring Additions

* Added the LMS Loader component. The LMS Loader manages the project communication with the pre-designed XR4GOOD LMS. It downloads the data from a specific lesson for the components to use if they are set to do so. It is a Singleton, so only one LMS Loader script can exist in a scene.
* Added the LRS Information component. This is a simple component that stores authentication data for any wished LRS. It is a Singleton, so only one LRS Information script can exist in a scene.
* The Slide and Quiz components can load the downloaded data from the LMS by the LMS Loader automatically if they are set to do so by setting the "use LMS Loader" flag to true.
* Added a variable on the Slide Component to specify a default slide for broken images
* Added a Tests folder to the package.

### Featuring Changes

* Changed Slide, Quiz and Audio components inspectors to accommodate the new LMS Loader addition.
* Changed StatementSenders inspectors to accommodate the new LRS Information addition.

### Bug Fixes

* Fixed a bug where the Slide Component were loading duplicate images on the last slide

# 1.1.1

2022-10-10

### Featuring Additions

* Each practical component now has a prefab for VR usage, using the STEAM VR Library
* Quiz Controller script now has a way to track the current selected answer

# 1.1.2

2022-10-12

### Featuring Additions

* Added tests for Audio Player Component
* Added tests for Quiz Component
* Added tests for Slide Component
* Added tests for LMS Loader Component

### Bug Fixes

* Adjusted component dependencies on editor instantiated variables. Now it instantiates any uninstantiated variables in the Start method
* Fixed Audio Player component not registering what actual audio index the component is reproducing

# 1.1.3

2022-11-03

### Featuring Additions

* Added xAPI Statements for Quiz component
* Added TinCan Extension support for all statements sent

### Featuring Changes

* Now every statement created by the components have an extensions specifying the component identifier
* Changed non-VR slide prefab to send the statements after the action has been done
* Changed VR and non-VR quiz component prefabs to send the new statements added in this patch
