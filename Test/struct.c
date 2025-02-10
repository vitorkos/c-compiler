#include <stdio.h>

struct Ponto {
    int x;
    int y;
};

int main() {
    struct Ponto p;
    p.x = 10;
    p.y = 20;
    printf("Ponto: (%d, %d)\n", p.x, p.y);
    return 0;
}