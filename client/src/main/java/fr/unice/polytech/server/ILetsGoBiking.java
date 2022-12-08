
package fr.unice.polytech.server;

import javax.jws.WebMethod;
import javax.jws.WebParam;
import javax.jws.WebResult;
import javax.jws.WebService;
import javax.xml.bind.annotation.XmlSeeAlso;
import javax.xml.ws.RequestWrapper;
import javax.xml.ws.ResponseWrapper;


/**
 * This class was generated by the JAX-WS RI.
 * JAX-WS RI 2.3.2
 * Generated source version: 2.2
 * 
 */
@WebService(name = "ILetsGoBiking", targetNamespace = "http://tempuri.org/")
@XmlSeeAlso({
    ObjectFactory.class
})
public interface ILetsGoBiking {


    /**
     * 
     * @param origin
     * @param destination
     * @return
     *     returns fr.unice.polytech.server.ArrayOfstring
     */
    @WebMethod(operationName = "GetItinerary", action = "http://tempuri.org/ILetsGoBiking/GetItinerary")
    @WebResult(name = "GetItineraryResult", targetNamespace = "http://tempuri.org/")
    @RequestWrapper(localName = "GetItinerary", targetNamespace = "http://tempuri.org/", className = "fr.unice.polytech.server.GetItinerary")
    @ResponseWrapper(localName = "GetItineraryResponse", targetNamespace = "http://tempuri.org/", className = "fr.unice.polytech.server.GetItineraryResponse")
    public ArrayOfstring getItinerary(
        @WebParam(name = "origin", targetNamespace = "http://tempuri.org/")
        String origin,
        @WebParam(name = "destination", targetNamespace = "http://tempuri.org/")
        String destination);

}
