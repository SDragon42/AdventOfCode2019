import utils

# classes
#--------------------------------------------------------------------------------
class Offsets:
    x: int
    y: int

    def __init__(self, xOffset, yOffset):
        self.x = xOffset
        self.y = yOffset

directionOffsets: list[Offsets] = [
    Offsets(-1, -1),
    Offsets(0, -1),
    Offsets(1, -1),
    Offsets(1, 0),
    Offsets(1, 1),
    Offsets(0, 1),
    Offsets(-1, 1),
    Offsets(-1, 0),
]


# type aliases
#--------------------------------------------------------------------------------
table = list[list[str]]





# helper functions
#--------------------------------------------------------------------------------
def clone_table(seats: table) -> table:
    cloned: table = []
    for y in seats:
        cloned.append(y.copy())
    return cloned


def input_list_to_table(input: list[str]) -> table:
    seats: table = []
    for y in input:
        seats.append(list(y))
    return seats


def show_seat_map(seats: table, title: str):
    print(title)
    for y in seats:
        print(y)
    print("")


def count_occupied_seats(seats: table) -> int:
    total = 0
    for y in seats:
        for x in y:
            if x == "#":
                total += 1
    return total


def get_first_seat_in_path(x: int, y: int, seats: table, offset: Offsets) -> str:
    x += offset.x
    y += offset.y
    if x < 0 or y < 0:
        return "."
    if y >= len(seats):
        return "."
    if x >= len(seats[y]):
        return "."
    if seats[y][x] != ".":
        return seats[y][x]
    return get_first_seat_in_path(x, y, seats, offset)
    

# common rules
#--------------------------------------------------------------------------------
def rule_floor(x: int, y: int, seats: table) -> str:
    return seats[y][x]


# rules for Part 1
#--------------------------------------------------------------------------------
def rule_empty_seat_part1(x: int, y: int, seats: table) -> str:
    y1 = y - 1
    while y1 <= y + 1 and y1 < len(seats):
        if y1 < 0:
            y1 = 0

        x1 = x - 1
        while x1 <= x + 1 and x1 < len(seats[y]):
            if x1 < 0:
                x1 = 0

            if x1 != x or y1 != y:
                if seats[y1][x1] == "#":
                    return seats[y][x]

            x1 += 1
        y1 += 1
    return "#"


def rule_occupied_seat_part1(x: int, y: int, seats: table) -> str:
    y1 = y - 1
    numOccupied = 0
    while y1 <= y + 1 and y1 < len(seats):
        if y1 < 0:
            y1 = 0

        x1 = x - 1
        while x1 <= x + 1 and x1 < len(seats[y]):
            if x1 < 0:
                x1 = 0

            if x1 != x or y1 != y:
                if seats[y1][x1] == "#":
                    numOccupied += 1

            if numOccupied >= 4:
                return "L"

            x1 += 1
        y1 += 1
    return seats[y][x]


# rules for Part 2
#--------------------------------------------------------------------------------
def rule_empty_seat_part2(x: int, y: int, seats: table) -> str:
    for offset in directionOffsets:
        if get_first_seat_in_path(x, y, seats, offset) == "#":
            return seats[y][x]
    return "#"


def rule_occupied_seat_part2(x: int, y: int, seats: table) -> str:
    numOccupied = 0
    for offset in directionOffsets:
        if get_first_seat_in_path(x, y, seats, offset) == "#":
            numOccupied += 1
    if numOccupied >= 5:
        return "L"
    return seats[y][x]


# Common
#--------------------------------------------------------------------------------
def apply_rules_to_seats(seats: table, rules: dict) -> int:
    numChanges = 0
    y = 0

    oldSeats = clone_table(seats)
    while y < len(oldSeats):
        x = 0
        while x < len(oldSeats[y]):
            currentSeat = oldSeats[y][x]
            newSeat = currentSeat
            if currentSeat in rules:
                newSeat = rules[currentSeat](x, y, oldSeats)

            if newSeat != currentSeat:
                seats[y][x] = newSeat
                numChanges += 1
            
            x += 1
        y += 1

    return numChanges



def run(title: str, input: list[str], correctResult: int, rules: dict):
    seats = input_list_to_table(input)

    numChanges = -1
    numPasses = 0


    # show_seat_map(seats, "Initial")
    while(numChanges != 0):
        numPasses += 1
        numChanges = apply_rules_to_seats(seats, rules)
        # show_seat_map(seats, f"Pass: {numPasses}")

    result = count_occupied_seats(seats)
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    print("---- Day 11: Seating System ----")

    rules_part1 = {
        ".": rule_floor,
        "L": rule_empty_seat_part1,
        "#": rule_occupied_seat_part1,
    }

    run("Test Case 1",
        utils.read_input_as_list(11, "example1"),
        37,
        rules_part1)
    run("problem",
        utils.read_input_as_list(11, "input"),
        2238,
        rules_part1)

    print("---- part 2 ----")

    rules_part2 = {
        ".": rule_floor,
        "L": rule_empty_seat_part2,
        "#": rule_occupied_seat_part2,
    }

    run("Test Case 1",
        utils.read_input_as_list(11, "example1"),
        26,
        rules_part2)
    run("problem",
        utils.read_input_as_list(11, "input"),
        2013,
        rules_part2)