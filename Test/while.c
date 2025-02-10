#include <stdio.h>

int main() {
    int contador = 0;

    while (contador < 3) {
        printf("Contador: %d\n", contador);
        contador = contador  + 1;
    }

    return 0;
}