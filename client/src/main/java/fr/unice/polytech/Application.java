package fr.unice.polytech;

import lombok.extern.slf4j.Slf4j;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;
import org.springframework.oxm.jaxb.Jaxb2Marshaller;

@SpringBootApplication
@Slf4j
public class Application {

    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }

    @Bean
    public Jaxb2Marshaller marshaller() {
        Jaxb2Marshaller marshaller = new Jaxb2Marshaller();
        // this package must match the package in the <generatePackage> specified in
        // pom.xml
        marshaller.setContextPath("fr.unice.polytech.server");
        return marshaller;
    }

    @Bean
    public SoapClient soapClient(Jaxb2Marshaller marshaller) {
        SoapClient client = new SoapClient();
        client.setDefaultUri("http://dneonline.com/calculator.asmx");
        client.setMarshaller(marshaller);
        client.setUnmarshaller(marshaller);
        return client;
    }

    @Bean
    CommandLineRunner lookup(SoapClient client) {
        return args -> {
            int r = client.iAdd(3, 5);
            log.info("3 + 5 = " + r);
            r = client.iSubtract(3, 5);
            log.info("3 - 5 = " + r);
            r = client.iMultiply(3, 5);
            log.info("3 * 5 = " + r);
            r = client.iDivide(3, 5);
            log.info("3 / 5 = " + r);
        };
    }

}
