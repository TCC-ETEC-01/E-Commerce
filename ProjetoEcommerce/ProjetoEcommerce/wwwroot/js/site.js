
// script para o menu aside
 
//Expandir o menu
var btnExp = document.querySelector('#btn-exp');
var menuSide = document.querySelector('.sidebar');

btnExp.addEventListener('click', function () {
    menuSide.classList.toggle('expandir');

    // Alternar o ícone entre setinha direita e esquerda
    if (btnExp.classList.contains('bi-arrow-right-circle-fill')) {
        btnExp.classList.replace('bi-arrow-right-circle-fill', 'bi-arrow-left-circle-fill');
    } else {
        btnExp.classList.replace('bi-arrow-left-circle-fill', 'bi-arrow-right-circle-fill');
    }
});
