package fr.unice.polytech;

import fr.unice.polytech.server.ILetsGoBiking;
import fr.unice.polytech.server.LetsGoBiking;

public class Application {
    public static void main(String[] args) {
        LetsGoBiking letsGoBiking = new LetsGoBiking();
        ILetsGoBiking proxy = letsGoBiking.getBasicHttpBindingILetsGoBiking();
        String itinerary = proxy.getItinerary(
                "2 Place de Paris, 69009 Lyon",
                "Bd Jard. Zoologique, 13004 Marseille");
        System.out.println("itinerary: " + itinerary);
    }
}
