#include <stdio.h>

void soma(int a, int b) {
    int soma = a + b;
    printf("Soma: %d\n", soma); 
}

int main() {
    int a, b;
    printf("Digite o primeiro numero: ");
    scanf("%d", &a);
    printf("Digite o segundo numero: ");
    scanf("%d", &b);
    soma(a, b);
}