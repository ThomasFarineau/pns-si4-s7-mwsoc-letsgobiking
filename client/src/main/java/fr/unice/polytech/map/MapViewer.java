package fr.unice.polytech.map;

import fr.unice.polytech.Instruction;
import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.OSMTileFactoryInfo;
import org.jxmapviewer.cache.FileBasedLocalCache;
import org.jxmapviewer.input.CenterMapListener;
import org.jxmapviewer.input.PanKeyListener;
import org.jxmapviewer.input.PanMouseInputListener;
import org.jxmapviewer.input.ZoomMouseWheelListenerCursor;
import org.jxmapviewer.painter.CompoundPainter;
import org.jxmapviewer.painter.Painter;
import org.jxmapviewer.viewer.*;

import javax.swing.*;
import javax.swing.event.MouseInputListener;
import java.awt.*;
import java.io.File;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;
import java.util.logging.Logger;
import java.util.stream.Collectors;

public class MapViewer extends JFrame {
    static Logger logger = Logger.getLogger(MapViewer.class.getName());

    MapViewer instance = this;

    public MapViewer(List<Instruction> instructions) {
        this.setSize(800, 600);
        this.setTitle("LetsGoBiking - Map");
        this.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);

        TileFactoryInfo osmTileFactoryInfo = new OSMTileFactoryInfo();
        DefaultTileFactory defaultTileFactory = new DefaultTileFactory(osmTileFactoryInfo);

        File cacheDir = new File(System.getProperty("user.home") + File.separator + ".jxmapviewer2");
        defaultTileFactory.setLocalCache(new FileBasedLocalCache(cacheDir, false));

        JXMapViewer mapViewer = new JXMapViewer();

        mapViewer.setTileFactory(defaultTileFactory);

        List<GeoPosition> positions = instructions.stream().map(Instruction::geoPosition).toList();
        logger.info("Loaded " + positions.size() + " positions in the painter");

        // Create a track from the geo-positions
        RoutePainter routePainter = new RoutePainter(positions);

        // Create waypoints from the geo-positions
        Set<DefaultWaypoint> waypoints = new HashSet<>();
        waypoints.add(new DefaultWaypoint(positions.get(0)));
        waypoints.add(new DefaultWaypoint(positions.get(positions.size() - 1)));

        // Create a waypoint painter that takes all the waypoints
        WaypointPainter<DefaultWaypoint> waypointPainter = new WaypointPainter<>();
        waypointPainter.setWaypoints(waypoints);

        // Create a compound painter that uses both the route-painter and the waypoint-painter
        List<Painter<JXMapViewer>> painters = new ArrayList<>();
        painters.add(routePainter);
        painters.add(waypointPainter);

        CompoundPainter<JXMapViewer> painter = new CompoundPainter<>(painters);
        mapViewer.setOverlayPainter(painter);

        // Set the focus
        mapViewer.setZoom(7);
        mapViewer.setAddressLocation(positions.get(0));

        // Add interactions
        MouseInputListener mia = new PanMouseInputListener(mapViewer);
        mapViewer.addMouseListener(mia);
        mapViewer.addMouseMotionListener(mia);
        mapViewer.addMouseListener(new CenterMapListener(mapViewer));
        mapViewer.addMouseWheelListener(new ZoomMouseWheelListenerCursor(mapViewer));
        mapViewer.addKeyListener(new PanKeyListener(mapViewer));

        this.setLayout(new BorderLayout());
        this.add(mapViewer);
        this.setVisible(true);
    }
}
