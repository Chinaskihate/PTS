import {Progress} from "../../data/models/TestResult";
import {Difficult} from "../../data/models/Test";

export function getDifficultColor(difficult: Difficult): string {
    switch (difficult) {
        case Difficult.Easy:
            return "#0EE590"
        case Difficult.Normal:
            return "#E57D0E"
        case Difficult.Hard:
            return "#E12323"
        default:
            return "#000000"
    }
}

export function getProgressColor(progress: Progress): string {
    switch (progress) {
        case Progress.OK:
            return "#0EE590";
        case Progress.Incorrect:
            return "#E12323";
        case Progress.InProgress:
            return "#8f8f8f";
        default:
            return "#000000"
    }
}

export function getRandomColor(index: number) {
    const colors = ["#E12323", "#E57D0E", "#0EE590", "#0EA7E5"]
    return colors[index % colors.length]
}
