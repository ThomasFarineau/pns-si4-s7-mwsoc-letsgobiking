package fr.unice.polytech;

import fr.unice.polytech.server.ILetsGoBiking;
import fr.unice.polytech.server.LetsGoBiking;

public class Application {
    public static void main(String[] args) {
        LetsGoBiking letsGoBiking = new LetsGoBiking();
        ILetsGoBiking proxy = letsGoBiking.getBasicHttpBindingILetsGoBiking();
        String itinerary = proxy.getItinerary(
                "4 rue Panait Istrati, 06000 Nice",
                "181 bd de la Madeleine, 06000 Nice");
        System.out.println("itinerary: " + itinerary);
    }
}
