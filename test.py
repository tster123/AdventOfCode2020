
hash = dict()
for i in range(0, 10000000):
    hash[repr(i)] = "foo"

sum = 0;
for i in range (5000000, 1500000):
    if repr(i) in hash.keys():
        sum += 1

print(sum)