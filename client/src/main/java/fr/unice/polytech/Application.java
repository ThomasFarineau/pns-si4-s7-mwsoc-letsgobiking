package fr.unice.polytech;

import fr.unice.polytech.map.MapViewer;
import fr.unice.polytech.server.ArrayOfstring;
import fr.unice.polytech.server.ILetsGoBiking;
import fr.unice.polytech.server.LetsGoBiking;
import org.jxmapviewer.viewer.GeoPosition;

import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.logging.Logger;

public class Application {

    static Logger logger = Logger.getLogger(Application.class.getName());

    public static void main(String[] args) {

        if (!isUptime()) {
            logger.severe("Server is down");
            System.exit(1);
        }

        LetsGoBiking letsGoBiking = new LetsGoBiking();
        ILetsGoBiking proxy = letsGoBiking.getBasicHttpBindingILetsGoBiking();

        String originAddress = askForAddress("origin");
        String destinationAddress = askForAddress("destination");

        logger.info("Calling the web service...");
        ArrayOfstring arrayOfstring = proxy.getItinerary(originAddress, destinationAddress);
        logger.info("Web service called.");
        List<String> instructions = new ArrayList<>(arrayOfstring.getString());
        logger.info("Parsing the instructions...");
        List<Instruction> instructionList = getInstructions(instructions);
        new MapViewer(instructionList);
        instructionList.forEach(System.out::println);
    }

    public static boolean isUptime() {
        try {
            LetsGoBiking letsGoBiking = new LetsGoBiking();
            letsGoBiking.getBasicHttpBindingILetsGoBiking();
        } catch (Exception e) {
            return false;
        }
        return true;
    }

    private static String askForAddress(String askedAddress, String... args) {
        if (args.length > 0) {
            return args[0];
        }
        String address = "";
        Scanner scanner = new Scanner(System.in);
        while (address.isEmpty()) {
            System.out.print("Please enter the " + askedAddress + " address: ");
            address = scanner.nextLine();
        }
        return address;
    }

    public static List<Instruction> getInstructions(List<String> instructions) {
        List<Instruction> instructionList = new ArrayList<>();
        for (String instruction : instructions) {
            logger.info("Parsing instruction: " + instruction);
            String[] split = instruction.split("§");
            String direction = split[0];
            Double distance = Double.parseDouble(split[1].replace(",", "."));
            GeoPosition geoPosition = new GeoPosition(Double.parseDouble(split[2].replace(",", ".")), Double.parseDouble(split[3].replace(",", ".")));
            instructionList.add(new Instruction(direction, distance, geoPosition, split[0].startsWith("Arrive")));
        }
        return instructionList;
    }
}
