#include <stdio.h>

int main() {
    char str[100];
    printf("Digite uma string: ");
    fgets(str);
    fputs("Você digitou:");
    fputs(str);
    return 0;
}