package fr.unice.polytech;

import org.jxmapviewer.viewer.GeoPosition;

public record Instruction(String text, Double distance, GeoPosition geoPosition, boolean isWaypoint) {

    @Override
    public String toString() {
        return text + " (" + distance + "m)";
    }
}
