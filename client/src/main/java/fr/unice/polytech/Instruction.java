package fr.unice.polytech;

import org.jxmapviewer.viewer.GeoPosition;

import java.util.List;

public record Instruction(String text, Double distance, List<GeoPosition> geoPosition, boolean isWaypoint) {

    @Override
    public String toString() {
        return text + " (" + distance + "m)";
    }
}
