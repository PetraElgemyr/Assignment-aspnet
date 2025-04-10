document.addEventListener('DOMContentLoaded', () => {
    initAddForms();
    initUpdateForms();
})


function initAddForms() {
    const forms = document.querySelectorAll('[form-type="add"]')
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            //clearFormErrorMessages(form)

            const formData = new FormData(form)
            console.log("formdata här:", form);

            try {
                const res = await fetch(form.action, {
                    method: "post",
                    body: formData
                })

                if (res.ok) {
                    const modal = form.closest('.modal')
                    if (modal)
                        closeFormModal(modal)

                    window.location.reload()
                }
                else if (res.status === 400) {
                    const data = await res.json()
                    if (data.errors) {
                        console.log(data.errors)
                        showValidationErrors(data.errors)
                        //addFormErrorMessages(data.errors, form)
                    }
                }
                else if (res.status === 409) {
                    alert('Already exists')
                }
                else {
                    alert('Unable to create')
                }
            }
            catch {

            }
        })
    })
}


function initUpdateForms() {
    const forms = document.querySelectorAll('[form-type="update"]')
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            //clearFormErrorMessages(form)

            const formData = new FormData(form)
            console.log("formdata här:", form);

            try {
                const res = await fetch(form.action, {
                    method: "put",
                    body: formData
                })

                if (res.ok) {
                    const modal = form.closest('.modal')
                    if (modal)
                        closeFormModal(modal)

                    window.location.reload()
                }
                else if (res.status === 400) {
                    const data = await res.json()
                    if (data.errors) {
                        console.log(data.errors)
                        showValidationErrors(data.errors)
                        //addFormErrorMessages(data.errors, form)
                    }
                }
                else if (res.status === 404) {
                    alert('Could not found entity to update')
                }
                else {
                    alert('Unable to update')
                }
            }
            catch {

            }
        })
    })
}




function clearFormErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error')
    })

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = ''
        span.classList.remove('field-validation-error')
    })
}

//function findInputByKey(form, key) {
//    return form.querySelector(`[name*="${key}"]`);
//}

//function addFormErrorMessages(errors, form) {
//    Object.keys(errors).forEach(key => {
//        const input = findInputByKey(form, key);

//        //const input = form.querySelector(`[name="${key}"]`) // här ska jag kolla om det name contains key (clientId) för just nu är name=AddProjectViewModel_ClientId
//        if (input) {
//            input.classList.add('input-validation-error')
//        }

//        const span = form.querySelector(`[data-valmsg-for="${key}"]`)
//        if (span) {
//            span.innerText = errors[key].join(' ')
//            span.classList.add('field-validation-error')
//        }
//    })
//}

function closeFormModal(modal) {
    if (modal) {
        modal.classList.remove('show')


        modal.querySelectorAll('form').forEach(form => {
            form.reset()
        })
    }
}


function showValidationErrors(errors) {
    for (const field in errors) {
        const messages = errors[field];
        const fieldElement = document.querySelector(`[name="${field}"]`);

        if (fieldElement) {
            let errorElement = fieldElement.parentElement.querySelector(".field-validation-error");
            if (!errorElement) {
                errorElement = document.createElement("div");
                errorElement.className = "field-validation-error";
                fieldElement.parentElement.appendChild(errorElement);
            }
            errorElement.innerHTML = messages.join("<br/>");
        }
    }
}