const modal = document.getElementById("myModal");

const modalImg = document.getElementById("modal-image");
const elements = document.getElementsByClassName("detailImage");

for (let i = 0; i < elements.length; i++) {
    elements[i].onclick = function(){
        modal.style.display = "block";
        modalImg.src = this.src;
    }
}

const span = document.getElementsByClassName("close")[0];

span.onclick = function() {
    modal.style.display = "none";
} 