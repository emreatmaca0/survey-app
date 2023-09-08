var element=document.getElementById("1");



document.getElementById("add_button").addEventListener("click", addQuestions);



function addQuestions() {
    //element.parentNode.insertBefore(copyElement,element.nextSibling);
    var sonElement = document.getElementById("form1").lastElementChild.previousElementSibling;
    var copyElement=element.cloneNode(true);
    //copyElement.id=sonElement.id+1;
    for (let index = 1; index < 10; index++) {
        if (sonElement.id==index) {
            copyElement.id=index+1;
            copyElement.firstElementChild.placeholder="Soru-"+(index+1);
            copyElement.firstElementChild.id = "question-" + (index + 1);
            copyElement.firstElementChild.name = "Question_Text" + (index+1);
            copyElement.firstElementChild.value="";
            copyElement.firstElementChild.nextElementSibling.firstElementChild.firstElementChild.id="question-"+(index+1)+"_"+"answer-1";
            copyElement.firstElementChild.nextElementSibling.firstElementChild.nextElementSibling.firstElementChild.id="question-"+(index+1)+"_"+"answer-2";
            copyElement.firstElementChild.nextElementSibling.firstElementChild.nextElementSibling.nextElementSibling.firstElementChild.id="question-"+(index+1)+"_"+"answer-3";
            copyElement.firstElementChild.nextElementSibling.firstElementChild.nextElementSibling.nextElementSibling.nextElementSibling.firstElementChild.id="question-"+(index+1)+"_"+"answer-4";


            copyElement.firstElementChild.nextElementSibling.firstElementChild.firstElementChild.name = "Question" + (index+1) + "-Answer_One"//"question-"+(index+1)+"_"+"answer-1";
            copyElement.firstElementChild.nextElementSibling.firstElementChild.nextElementSibling.firstElementChild.name = "Question" + (index + 1) + "-Answer_Two";
            copyElement.firstElementChild.nextElementSibling.firstElementChild.nextElementSibling.nextElementSibling.firstElementChild.name = "Question" + (index + 1) + "-Answer_Three";
            copyElement.firstElementChild.nextElementSibling.firstElementChild.nextElementSibling.nextElementSibling.nextElementSibling.firstElementChild.name = "Question" + (index + 1) + "-Answer_Four";


            copyElement.firstElementChild.nextElementSibling.firstElementChild.firstElementChild.value="";
            copyElement.firstElementChild.nextElementSibling.firstElementChild.nextElementSibling.firstElementChild.value="";
            copyElement.firstElementChild.nextElementSibling.firstElementChild.nextElementSibling.nextElementSibling.firstElementChild.value="";
            copyElement.firstElementChild.nextElementSibling.firstElementChild.nextElementSibling.nextElementSibling.nextElementSibling.firstElementChild.value="";
            sonElement.after(copyElement);
        }
        
    }
    
}

