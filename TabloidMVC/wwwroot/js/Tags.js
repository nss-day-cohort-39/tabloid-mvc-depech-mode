/***** This script controls allows users to easily see the tags that they've added or removed *****/

const tagInput = document.querySelector("#tags");
const tagContainer = document.querySelector("#tagContainer");
const addTagButton = document.querySelector("#addTagButton");
const storedTags = document.querySelector("#storedTags"); //a hidden form field that holds a string version of the tags

const xIcon = `<svg class="bi bi-x" width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                  <path fill-rule="evenodd" d="M11.854 4.146a.5.5 0 0 1 0 .708l-7 7a.5.5 0 0 1-.708-.708l7-7a.5.5 0 0 1 .708 0z"/>
                  <path fill-rule="evenodd" d="M4.146 4.146a.5.5 0 0 0 0 .708l7 7a.5.5 0 0 0 .708-.708l-7-7a.5.5 0 0 0-.708 0z"/>
                </svg>`;

//array that will store the tags
const tagArray = [];

//function that renders the tags on the DOM
const renderTags = () => {
    tagContainer.innerHTML = tagArray.map((tag, index) => {
        return `<a href="#" class="badge badge-light p-2 mr-2" id="tag-${index}" >${tag} ${xIcon}</a>`;
    }).join('');
    storedTags.value = tagArray.map(tag => {
        return tag;
    }).join(',');
}

//get already saved tags, add them to the array, and render them to the DOM
for (const tag of storedTags.value.split(",")) {
    if (tag != "") {
        tagArray.push(tag);
    }
}
renderTags();

//event listener to add the tag
addTagButton.addEventListener("click", event => {
    const newTag = tagInput.value;
    if (newTag != "" && !tagArray.includes(newTag) && newTag != "Select...") { //don't add a blank tag or duplicate
        tagArray.push(newTag);
        renderTags();
    }
    tagInput.value = "Select...";
});

//event listener to remove the tag from the array
tagContainer.addEventListener("click", event => {
    if (event.target.id.startsWith("tag-")) {
        const [prefix, tagId] = event.target.id.split("-");
        tagArray.splice(tagId, 1);
        renderTags();
    }
});

//submit the form on button click
document.querySelector("#submitButton").addEventListener("click", event => {
    document.querySelector("#tagForm").submit();
})

