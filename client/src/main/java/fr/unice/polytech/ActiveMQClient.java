package fr.unice.polytech;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageListener;
import javax.jms.TextMessage;

import static fr.unice.polytech.Application.parseInstruction;
import static fr.unice.polytech.Application.separator;

public class ActiveMQClient implements MessageListener {
    @Override
    public void onMessage(Message message) {
        if (message instanceof TextMessage m) {
            try {
                Instruction instruction = parseInstruction(m.getText().split(separator));
                System.out.println(instruction);
            } catch (JMSException e) {
                e.printStackTrace();
            }
        }
    }
}
