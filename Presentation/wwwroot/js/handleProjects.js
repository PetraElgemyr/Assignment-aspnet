
function handleDeleteProjectRequestFromElement(element) {
    const projectId = element.getAttribute("data-project-id");
    handleDeleteProjectRequest(projectId);
}


async function handleDeleteProjectRequest(projectId) {
    try {
        console.log("klickat och skickar", projectId)

        const response =  await fetch(`/admin/projects/${projectId}`, {
            method: 'DELETE'
        })

        console.log("response är: ", response)

        if (response.ok) {
            window.location.reload();
        } else {
            console.log("något gick fel med att radera projekt");
        }

    }
    catch (error) {
        console.error("Fel vid borttagning:", error);
    }
}

