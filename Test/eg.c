#include <stdio.h>

void soma(int a, int b) {
    int soma = a + b;
    printf("Soma: %d\n", soma); 
}

//comentario
/*
comentarios
*/

int main() {
    int a, b;
    printf("n1: ");
    scanf("%d", &a);
    printf("n2: ");
    scanf("%d", &b);
    soma(a, b);
}