#include <iostream>
#include "functions.h"

using namespace std;

int main(){
    print_hello();
	char str [80];
    cout << endl;
    cout << "The factorial of 5 is " << factorial(5) << endl;
	printf ("Enter anything to quit!\n");
	scanf ("%s",str);
    return 0;
}
