using System;

namespace Portia.Lite
{
    public static class Docs
    {
        public static string NumericRelation =>
            "Numerical comparison logic (Equal, GreaterThan, ..) for filtering floating point numbers.";

        public static string StringRelation =>
            "Text-based filter (Contains, StartsWith, ..) used mostly for node / edge Type processes.";

        public static string BooleanCondition =>
            "Logical Boolean Toggle: If True, the rule captures elements that satisfy the condition. " +
            "If False, the rule is inverted to capture elements that do not.";

        public static string MatchAll =>
            "Similarly to the Gate component, this Boolean operator only returns with True, if the given condition(s) is true " +
            "for ALL elements that are evaluated. If its False, it is enough that ANY of the evaluated elements match the condition(s)";

        public static string Gate =>
            "Boolean operator (AND, OR) to combine multiple conditions into one filter. " +
            "AND means that all conditions must be True, OR means that any of the conditions is enough to be True.";

        public static string NumericValue =>
            "The target numerical (double a.k.a. floating point number) value for mathematical filtering.";

        public static string StringValue =>
            "The target string value used for text-based filtering.";

        public static string PortiaGraph =>
            "The core building block of the Portia ecosystem, " +
            "a graph structure designed to manage complex graph logic with architectural data.";

        public static string PortiaComponent =>
            "The core building block of the Portia ecosystem, " +
            "designed to manage complex graph logic with architectural data " +
            "through customizable Task inputs." + Environment.NewLine +
            "Portia reorders the input Tasks in the following order (independent from the input order after" +
            "the first curve setting task):" + Environment.NewLine +
            Environment.NewLine +
            $"Foundation  /  {nameof(SetGraphByCurves)} & {nameof(LoadGraph)}" +
            Environment.NewLine +
            $"Identity  /  {nameof(SetNodeIndices)} & {nameof(SetEdgeIndices)}" +
            Environment.NewLine +
            $"Topology  /  {nameof(SetNodeTypes)} & {nameof(SetEdgeTypes)}" +
            Environment.NewLine +
            $"Selection  /  {nameof(FilterNodes)} & {nameof(FilterEdges)}" +
            Environment.NewLine +
            $"Validation  /  {nameof(VerifyNodes)} & {nameof(VerifyEdges)}" +
            Environment.NewLine + Environment.NewLine + "•••" +
            Environment.NewLine + Environment.NewLine +
            "The PORTIA ecosystem is developed and maintained by Bálint Füzes of FUZES/ARCH." +
            Environment.NewLine +
            "I am always happy to discuss new development ideas, roadmap suggestions, or " +
            "specific enterprise customization needs." + Environment.NewLine +
            " Whether you have technical feedback or just want to say hello, please feel free to reach out:" +
            Environment.NewLine + Environment.NewLine +
            "• Web: https://fuzesarch.com/" + Environment.NewLine +
            "• Direct: balint@fuzesarch.com | info@fuzesarch.com" +
            Environment.NewLine + "• Discord: https://discord.gg/GHXCYeX5" +
            Environment.NewLine + Environment.NewLine + Environment.NewLine +
            "You are also warmly invited to explore and contribute to the open-source core here:" +
            Environment.NewLine + "https://github.com/fuzesArch/Portia.Lite";

        public static string Condition =>
            "Defines a logical filter rule based on the relationship " +
            "with a selection relation type and a specific target value.";

        public static string DoubleCondition =>
            "Defines a logical filter rule based on the relationship " +
            "with a double-based selection relation type and a specific double target value.";

        public static string VectorCondition =>
            "Defines a logical filter rule based on the geometric relationship " +
            "between two vectors.";

        public static string StringCondition =>
            "Defines a logical filter rule based on the relationship " +
            "with a text-based relation type and a specific string target value.";

        public static string BoundaryCondition =>
            "Defines a logical filter based on the geometric relationship " +
            "between points and boundaries, where a boundary is a closed Brep evaluated for point containment.";

        public static string Feature =>
            "Graph payload: a user-defined name-value pair that gets " +
            "added to the selected Nodes or Edges in order to enrich them.";

        public static string FeatureValue =>
            "Node or Edge Feature payload: a user-defined value (numeric, boolean, etc.) " +
            "that is stored by its Name in a Feature. ";

        public static string NodeFeatures =>
            "Graph payloads: user-defined name-value pairs that get " +
            "added to the selected Nodes in order to enrich them.";

        public static string EdgeFeatures =>
            "Graph payloads: user-defined name-value pairs that get " +
            "added to the selected Edges in order to enrich them.";

        public static string Identity =>
            "Defines the primary identification rule for graph elements " +
            "using either a numerical Index or a text-based Type.";

        public static string Index =>
            "The specific numerical index used to identify and map " +
            "a unique element (node or edge) within the graph structure.";

        public static string Type =>
            "A user-defined text string used to categorize and map " +
            "specific elements (nodes or edges) within the graph structure.";

        public static string Rule =>
            "Defines a localized graph process that evaluates " +
            "nodes or edges based on their geometric and topological behavior.";

        public static string EdgeRules =>
            "Defines a localized graph process that evaluates " +
            "Edges based on their geometric and topological behavior.";

        public static string IndexRule =>
            "Defines a localized graph process that evaluates both " +
            "Nodes AND Edges based on their numerical Index.";

        public static string TypeRule =>
            "Defines a localized graph process that evaluates both " +
            "Nodes AND Edges based on their text-based Type value.";

        public static string NodeRules =>
            "Defines a localized graph process that evaluates " +
            "Nodes based on their geometric and topological behavior.";

        public static string NodeAdjacentEdgeType =>
            "A topological query that validates a Node based on the Type " +
            "of its connected Edges. Useful for identifying hybrid structural junctions, or " +
            "setting the Type of a Node based on its neighbour.";

        public static string NodeAdjacentEdgeVectorSimilarity =>
            "A topological query that validates a Node based on the outgoing vectors " +
            "of its connected Edges. Useful for identifying hybrid structural junctions, or " +
            "setting the Type of a Node based on its neighbouring Edge constellation.";

        public static string NodeInBoundary =>
            "A topological query that validates a Node based on its centroid's " +
            "containment by the input boundary Breps. Useful to set initial Node Types " +
            "or Indices for basic categorization.";

        public static string EdgeInBoundary =>
            "A topological query that validates an Edge based on its start or end point's " +
            "containment by the input boundary Breps. Useful to set initial Edge Types " +
            "or Indices for basic categorization.";

        public static string NodeAdjacency =>
            "A rule that evaluates a Node's direct neighbouring Edge count.";

        public static string NodeProximity =>
            "A rule that checks the spatial distance between Nodes " +
            "regardless of direct edge connections.";

        public static string NodeVectorScalarSum =>
            "A rule that calculates the resultant force or orientation of a node " +
            "by summing the directional vectors of all connected edges." +
            Environment.NewLine +
            "0.0 / Perfect Symmetry: A cross(X) or a star where every force is perfectly cancelled out by an opposite one." +
            Environment.NewLine +
            "Close to 0.0 /	Balanced Junction: A standard 'T1 junction or a straight line (180°). The vectors point in opposite directions, 'pulling' the node equally." +
            Environment.NewLine +
            "1.0 /	Unbalanced / Corner: A 90° corner with two edges.The vectors don't cancel out; they combine to point toward the 'outside' of the corner." +
            Environment.NewLine +
            "High ( > 2.0) / Acute Bunching: Many edges all pointing in roughly the same direction.The node is 'heavily weighted' toward one side.";

        public static string IsLeafNode =>
            "A binary validation rule that identifies 'leaf' nodes—elements " +
            "with only one connection—at the termination of a graph branch.";

        public static string EdgeSimilarity =>
            "A geometric rule that compares an Edge's start tangent vector to the input vectors by the input conditions.";

        public static string EdgeCurveLength =>
            "A rule that evaluates the physical length of an Edge' curve.";

        public static string EdgeStartEndDistance =>
            "A rule that evaluates the virtual length between the Start and End Node Centroids of an Edge.";

        public static string StartDegree =>
            "A rule that evaluates the direct neighbouring Edge count of an Edge's start Node.";

        public static string EndDegree =>
            "A rule that evaluates the direct neighbouring Edge count of an Edge's end Node.";

        public static string EdgeStartIndex =>
            "A rule that evaluates / captures an Edge based on the numerical Index " +
            "of its start Node.";

        public static string EdgeStartType =>
            "A rule that evaluates / captures an Edge based on the text-based Type " +
            "of its end Node.";

        public static string EdgeEndIndex =>
            "A rule that evaluates / captures an Edge based on the numerical Index " +
            "of its end Node.";

        public static string EdgeEndType =>
            "A rule that evaluates / captures an Edge based on the text-based Type " +
            "of its end Node.";

        public static string IsLinearRule =>
            "A binary validation rule that determines if an Edge is perfectly straight " +
            "within document tolerance, distinguishing between linear members and curved geometry.";

        public static string Task =>
            "A specialized graph operation that executes " +
            "data modifications, queries or validation protocols by the Portia engine.";

        public static string SetGraphByCurves =>
            "A primary Task that translates geometric curves into graph-compatible " +
            "edge data for network processing.";

        public static string LoadGraph =>
            "A primary Task that activates a Graph instance from the input " +
            "Graph Goo wrapper.";

        public static string Amalgamate =>
            "Fuses an external Payload Graph into the " +
            "active Host Graph based on topological Target and Anchor matches.";

        public static string Solve =>
            "Executes the different logics on the Graph supplied and " +
            "specified concretely by the different Solvers.";

        public static string SetNodeIndices =>
            "A modification Task that assigns specific Index integer values to " +
            "existing graph Nodes for categorization.";

        public static string SetEdgeIndices =>
            "A modification Task that assigns specificIndex integer values to " +
            "existing graph Edges for categorization.";

        public static string SetNodeTypes =>
            "A modification Task that assigns specific Type string values to " +
            "existing graph Nodes for categorization.";

        public static string SetEdgeTypes =>
            "A modification Task that assigns specific Type string values to " +
            "existing graph Edges for categorization.";

        public static string SetNodeFeatures =>
            "A modification Task that assigns specific Feature values to " +
            "existing graph Nodes in order to enrich the data stored in them.";

        public static string SetEdgeFeatures =>
            "A modification Task that assigns specific Feature values to " +
            "existing graph Edges in order to enrich the data stored in them.";

        public static string FilterNodes =>
            "A retrieval (query) Task that extracts Node identity values and geometry data " +
            "from the graph for downstream modeling.";

        public static string FilterEdges =>
            "A retrieval (query) Task that extracts Edge identity values and geometry data " +
            "from the graph for downstream modeling.";

        public static string VerifyNodes =>
            "A validation Task that checks Nodes against selected logic rules " +
            "to ensure structural or topological integrity.";

        public static string VerifyEdges =>
            "A validation Task that checks Edges against selected logic rules " +
            "to ensure structural or topological integrity.";

        public static string Curves =>
            "The input geometric curves used to generate the " +
            "base network topology for the Portia engine.";

        public static string InitialEdgeTypes =>
            "The optional Type strings assigned to the Edges during the " +
            "initial graph generation process, used for categorization.";

        public static string Indices =>
            "The specific integer-based Index values to be assigned to the " +
            "elements that get captured by the current input Rule.";

        public static string Types =>
            "The specific text-based Type values to be assigned to the " +
            "elements that get captured by the current input Rule.";

        public static string NodeRulesToVerify =>
            "The graph logic rules that verify the topological or " +
            "geometric integrity of the Nodes that are captured " +
            "by the input Node rules!";

        public static string EdgeRulesToVerify =>
            "The graph logic rules that verify the topological or " +
            "geometric integrity of the Edges that are captured " +
            "by the input Edge rules!";

        public static string TargetNodeRules =>
            "Rules defining the exact insertion points " +
            "on the host Graph. (Must be Node Rules).";

        public static string AnchorNodeRules =>
            "Rules defining the exact receiving connection points " +
            "on the Payload Graph. (Must be Node Rules).";

        public static string Name =>
            "A unique identifier used to label logic results and " +
            "dynamically name the corresponding Portia output fields for easy tracking.";

        public static string AngleTolerance =>
            "Allowed angle deviation between vectors IN DEGREES. Used during similarity comparison between " +
            "outgoing Node vectors (constellations).";

        public static string Bidirectional =>
            "When enabled, the rule validates the connection regardless of edge direction, " +
            "checking both start-to-end and end-to-start orientations.";

        public static string CompositeRule =>
            "A sophisticated rule rule that combines multiple rules " +
            "using a boolean Gate logic in order to capture graph Nodes or Edges.";

        public static string Boundary =>
            "Boundary Brep used to define the spatial volumes " +
            "for Node centroid containment checks.";

        public static string StrictlyIn =>
            "A boolean toggle that determines if elements lying exactly on the " +
            "Brep boundary surface are included in the selection or not.";

        public static string Decode =>
            "A helper component used to unlock the details of the different Portia elements. " +
            "Nodes, Edges, Graphs and the FeatureSets (of Nodes and Edges) can be deconstructed into " +
            "their constituent parts. The items passed around are wrapped in the " +
            "Grasshopper-native Goo equivalents - hence the Goo suffix everywhere.";

        public static string DeconstructItem =>
            "A helper component used to unlock the details of a graph element (both Node or Edge), " +
            "extracting its identity, geometric content (Centroid or Curve), and its raw JSON format data. " +
            "Connect to Portia Nodes and Edges outputs!";

        public static string DeconstructGraph =>
            "A helper component used to unlock the details of a Portia graph, " +
            "extracting its Nodes and Edges, and its raw JSON format data. " +
            "Connect the outputs to item-related deconstruction components!";

        public static string DeconstructFeature =>
            "A helper component used to unlock the details of a Feature, hosted by an element (both Node or Edge), " +
            "extracting its Name key and Value pair";

        public static string GraphItemGoo =>
            "The proprietary 'Sovereign Goo' wrapper that encapsulates a graph item (Node or Edge), " +
            "carrying its full topological intelligence and metadata through the network.";

        public static string GraphNodeGoo =>
            "The proprietary 'Sovereign Goo' wrapper that encapsulates a graph Node, " +
            "carrying its full topological intelligence and metadata through the network.";

        public static string GraphEdgeGoo =>
            "The proprietary 'Sovereign Goo' wrapper that encapsulates a graph Edge, " +
            "carrying its full topological intelligence and metadata through the network.";

        public static string GraphGoo =>
            "The proprietary 'Sovereign Goo' wrapper that encapsulates a graph (with its Nodes and Edges), " +
            "carrying its full topological intelligence and metadata through the network.";

        public static string FeatureGoo =>
            "The proprietary 'Sovereign Goo' wrapper that encapsulates a Feature, " +
            "carrying its full key-value pair dictionary.";

        public static string Json =>
            "The universal, transaction-ready string representation of a graph element (Node or Edge), " +
            "allowing for seamless data exchange between Portia and external logic engines.";

        public static string Geometries =>
            "The projected geometric representation of an element: a Node's Centroid or Edge's Curve.";

        public static string Vector => "3D vector.";

        public static string Solvers =>
            "A collection of specific, complex logics " +
            "executed on the graph.";
    }
}