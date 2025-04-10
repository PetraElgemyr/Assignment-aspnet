
function handleDeleteProjectRequestFromElement(element) {
    const projectId = element.getAttribute("data-project-id");
    handleDeleteProjectRequest(projectId);
}


async function handleDeleteProjectRequest(projectId) {
    try {
        const response =  await fetch(`/admin/projects/${projectId}`, {
            method: 'DELETE'
        })


        if (response.ok) {
            window.location.reload();
        } else {
            alert("något gick fel med att radera projekt");

            console.log("något gick fel med att radera projekt");
        }

    }
    catch (error) {
        alert("något gick fel med att radera projekt");

        console.error("Fel vid borttagning:", error);
    }
}

