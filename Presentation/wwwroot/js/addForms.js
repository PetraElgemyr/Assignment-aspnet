document.addEventListener('DOMContentLoaded', () => {
    initClientForm();

})
function initClientForm() {
    //const form = document.querySelector('#clientForm')
    const forms = document.querySelectorAll('form')
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            clearFormErrorMessages(form)

            const formData = new FormData(form)

            try {
                const res = await fetch(form.action, {
                    method: 'post',
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
                        addFormErrorMessages(data.errors, form)
                    }
                }
                else if (res.status === 409) {
                    alert('Client already exists')
                }
                else {
                    alert('Unable to create new Client')
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

function addFormErrorMessages(errors, form) {
    Object.keys(errors).forEach(key => {
        const input = form.querySelector(`[name="${key}"]`)
        if (input) {
            input.classList.add('input-validation-error')
        }

        const span = form.querySelector(`[data-valmsg-for="${key}"]`)
        if (span) {
            span.innerText = errors[key].join(' ')
            span.classList.add('field-validation-error')
        }
    })
}

function closeFormModal(modal) {
    if (modal) {
        modal.classList.remove('show')


        modal.querySelectorAll('form').forEach(form => {
            form.reset()
        })
    }
}