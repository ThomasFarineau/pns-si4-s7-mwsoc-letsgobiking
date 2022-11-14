package fr.unice.polytech;

import fr.unice.polytech.server.*;
import org.springframework.ws.client.core.support.WebServiceGatewaySupport;
import org.springframework.ws.soap.client.core.SoapActionCallback;

public class SoapClient extends WebServiceGatewaySupport {

    public AddResponse add(int intA, int intB) {
        Add request = new Add();
        request.setIntA(intA);
        request.setIntB(intB);
        return (AddResponse) getWebServiceTemplate()
                .marshalSendAndReceive("http://dneonline.com/calculator.asmx",
                        request,
                        new SoapActionCallback("http://tempuri.org/Add"));
    }

    public SubtractResponse subtract(int intA, int intB) {
        Subtract request = new Subtract();
        request.setIntA(intA);
        request.setIntB(intB);
        return (SubtractResponse) getWebServiceTemplate()
                .marshalSendAndReceive("http://dneonline.com/calculator.asmx",
                        request,
                        new SoapActionCallback("http://tempuri.org/Subtract"));
    }

    public MultiplyResponse multiply(int intA, int intB) {
        Multiply request = new Multiply();
        request.setIntA(intA);
        request.setIntB(intB);
        return (MultiplyResponse) getWebServiceTemplate()
                .marshalSendAndReceive("http://dneonline.com/calculator.asmx",
                        request,
                        new SoapActionCallback("http://tempuri.org/Multiply"));
    }

    public DivideResponse divide(int intA, int intB) {
        Divide request = new Divide();
        request.setIntA(intA);
        request.setIntB(intB);
        return (DivideResponse) getWebServiceTemplate()
                .marshalSendAndReceive("http://dneonline.com/calculator.asmx",
                        request,
                        new SoapActionCallback("http://tempuri.org/Divide"));
    }

    public int iAdd(int intA, int intB) {
        return add(intA, intB).getAddResult();
    }

    public int iSubtract(int intA, int intB) {
        return subtract(intA, intB).getSubtractResult();
    }

    public int iMultiply(int intA, int intB) {
        return multiply(intA, intB).getMultiplyResult();
    }

    public int iDivide(int intA, int intB) {
        return divide(intA, intB).getDivideResult();
    }

}
