package fr.unice.polytech;

import fr.unice.polytech.server.*;
import org.springframework.ws.client.core.support.WebServiceGatewaySupport;
import org.springframework.ws.soap.client.core.SoapActionCallback;

import javax.xml.bind.JAXBElement;

import static fr.unice.polytech.CONST.NAMESPACE_URI;
import static fr.unice.polytech.CONST.SERVER_URI;

public class SoapClient extends WebServiceGatewaySupport {

    public GetItineraryResponse getItineraryResponse(String origin, String destination) {
        GetItinerary request = new GetItinerary();
        request.setOrigin(new JAXBElement<String>(new javax.xml.namespace.QName("http://tempuri.org", "origin"), String.class, origin));
        request.setDestination(new JAXBElement<String>(new javax.xml.namespace.QName("http://tempuri.org", "destination"), String.class, destination));

        return (GetItineraryResponse) getWebServiceTemplate()
                .marshalSendAndReceive(SERVER_URI, request,
                        new SoapActionCallback(NAMESPACE_URI + "GetItinerary"));
    }

    public String getItinerary(String origin, String destination) {
        GetItineraryResponse response = getItineraryResponse(origin, destination);
        return response.getGetItineraryResult().getValue();
    }

}
