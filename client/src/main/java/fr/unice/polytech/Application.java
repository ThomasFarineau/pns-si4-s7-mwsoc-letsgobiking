package fr.unice.polytech;

import fr.unice.polytech.map.MapViewer;
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

    static boolean debug = false;

    public static void main(String[] args) {

        if (!isUptime()) {
            logger.severe("Server is down");
            System.exit(1);
        }

        LetsGoBiking letsGoBiking = new LetsGoBiking();
        ILetsGoBiking proxy = letsGoBiking.getBasicHttpBindingILetsGoBiking();

        //2 Place de Paris, 69009 Lyon
        String originAddress = askForAddress("origin", "2 Place de Paris, 69009 Lyon");
        //25 Rue Georges Gouy, 69007 Lyon
        String destinationAddress = askForAddress("destination", "25 Rue Georges Gouy, 69007 Lyon");

        if (debug) logger.info("Calling the web service...");
        new Thread(() -> {
            if (debug) logger.info("Calling the web service...");
            proxy.getItinerary(originAddress, destinationAddress);
        }).start();
        if (debug) logger.info("Retrieving the instructions...");
        handleActiveMQMessages();
    }

    public static void handleActiveMQMessages() {
        List<String> unparsedInstructions = new ArrayList<>();

        ConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");
        Connection connect;
        try {
            connect = factory.createConnection();
            Session receiveSession = connect.createSession(false, javax.jms.Session.AUTO_ACKNOWLEDGE);
            Queue queue = receiveSession.createQueue("instructions");
            javax.jms.MessageConsumer qReceiver = receiveSession.createConsumer(queue);
            qReceiver.setMessageListener(message -> {
                if (message instanceof TextMessage m) {
                    try {
                        if (m.getText().equals("END")) {
                            printInstructions(unparsedInstructions);
                            qReceiver.close();
                            receiveSession.close();
                            connect.close();
                        }
                        unparsedInstructions.add(m.getText());
                    } catch (JMSException e) {
                        e.printStackTrace();
                    }
                }
            });
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

    public static void printInstructions(List<String> unparsedInstructions) {
        if(debug) logger.info("Parsing the instructions...");
        List<Instruction> instructionList = getInstructions(unparsedInstructions);
        IntStream.range(0, instructionList.size()).mapToObj(i -> "Step " + (i + 1) + " - " + instructionList.get(i)).forEach(System.out::println);
        new MapViewer(instructionList);
    }

    public static List<Instruction> getInstructions(List<String> instructions) {
        List<Instruction> instructionList = new ArrayList<>();
        for (String instruction : instructions) {
            if(debug) logger.info("Parsing instruction: " + instruction);
            String[] split = instruction.split(separator);
            instructionList.add(parseInstruction(split));
        }
        return instructionList;
    }

    public static Instruction parseInstruction(String[] split) {
        String direction = split[0];
        Double distance = Double.parseDouble(split[1].replace(",", "."));
        List<GeoPosition> geoPositions = new ArrayList<>();
        for (int i = 2; i < split.length; i++) {
            GeoPosition geoPosition = new GeoPosition(Double.parseDouble(split[i].split(",")[0]), Double.parseDouble(split[i].split(",")[1]));
            geoPositions.add(geoPosition);
        }
        return new Instruction(direction, distance, geoPositions, direction.startsWith("Arrive"));
    }
}
