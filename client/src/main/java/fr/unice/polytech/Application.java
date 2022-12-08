package fr.unice.polytech;

import fr.unice.polytech.map.MapViewer;
import fr.unice.polytech.server.ArrayOfstring;
import fr.unice.polytech.server.ILetsGoBiking;
import fr.unice.polytech.server.LetsGoBiking;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.jxmapviewer.viewer.GeoPosition;

import javax.jms.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.logging.Logger;
import java.util.stream.IntStream;

public class Application {

    static Logger logger = Logger.getLogger(Application.class.getName());
    static String separator = "§";

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
        IntStream.range(0, instructionList.size()).mapToObj(i -> "Step " + (i+1) + " - " + instructionList.get(i)).forEach(System.out::println);
        System.out.println("----------------------- ACTIVE MQ -----------------------");
        ConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");
        Connection connect;
        try {
            connect = factory.createConnection();
            Session receiveSession = connect.createSession(false,javax.jms.Session.AUTO_ACKNOWLEDGE);
            Queue queue = receiveSession.createQueue("instructions");
            javax.jms.MessageConsumer qReceiver = receiveSession.createConsumer(queue);
            ActiveMQClient client = new ActiveMQClient();
            qReceiver.setMessageListener(client);
            connect.start();
        } catch (JMSException e) {
            e.printStackTrace();
        }
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
            String[] split = instruction.split(separator);
            instructionList.add(parseInstruction(split));
        }
        return instructionList;
    }

    public static Instruction parseInstruction(String[] split) {
        String direction = split[0];
        Double distance = Double.parseDouble(split[1].replace(",", "."));
        GeoPosition geoPosition = new GeoPosition(Double.parseDouble(split[2].replace(",", ".")), Double.parseDouble(split[3].replace(",", ".")));
        return new Instruction(direction, distance, geoPosition, split[0].startsWith("Arrive"));
    }
}
