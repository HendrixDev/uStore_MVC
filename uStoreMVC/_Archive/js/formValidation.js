function validateForm(event) {
    //Get data input into the fields:
    var name = document.forms['main-contact-form']['name'].value;
    var lastname = document.forms['main-contact-form']['lastname'].value;
    var email = document.forms['main-contact-form']['email'].value;
    var subject = document.forms['main-contact-form']['subject'].value;
    var message = document.forms['main-contact-form']['message'].value;

    //output to the spans
    var nameVal = document.getElementById('nameVal');
    var lastnameVal = document.getElementById('lastnameVal');
    var emailVal = document.getElementById('emailVal');
    var subjectVal = document.getElementById('subjectVal');
    var messageVal = document.getElementById('messageVal');



    var emailRegEx = new RegExp(/^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/);
    //Test all conditions before sending the data.
    if (name.length == 0 || lastname.length == 0 || email.length == 0 || subject.length == 0 || message.length == 0) {
        if (name.length == 0 || !emailRegEx.test(email)) {
            nameVal.textContent = "* Name is required";
        }
        else {
            nameVal.textContent = "";
        }
        if (lastname.length == 0) {
            lastnameVal.textContent = "* Last Name is required";
        }
        else {
            lastnameVal.textContent = "";
        }

        if (email.length == 0) {
            emailVal.textContent = "* Email is required";
        }
        else {
            emailVal.textContent = "";
        }
        if (subject.length == 0) {
            subjectVal.textContent = "* Subject is required";
        }
        else {
            subjectVal.textContent = "";
        }
        if (message.length == 0) {
            messageVal.textContent = "* Message is required";
        }
        else {
            messageVal.textContent = "";
        }

        if (!emailRegEx.test(email) && email.length > 0) {
            emailVal.textContent = "*Invalid e-mail address";
        }
        if (emailRegEx.test(email) && email.length > 0) {
            emailVal.textContent = "";
        }
        //Prevent the submit event from occuring.
        event.preventDefault();

    }

}