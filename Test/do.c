#include <stdio.h>

int main() {
    int contador = 0;

    do {
        printf("Contador: %d\n", contador);
        contador = contador + 1;
    } while (contador < 3);

    return 0;
}