# Educational-K-Means-Clustering
An educational Unity application made to facilitate the visualization of K-Means Clustering in a 3-dimensional space.

## K-Means Algorithm
1) Number of clusters **K** is selected.
2) **K** distinct centroids are *randomly* generated (**K** initial points are randomly generated, then points are dispersed around them randomly)
3) Distance is measured between all points and the **K** initial centroids.
4) Each point is assigned a centroid based on lowest distance.
5) Centroid position is updated based on the center of its cluster.
6) Steps 3, 4 and 5 are repeated until the calculated SSE between two consecutive iterations stays the same.

## K-Means++
1) One center is uniformly chosen at random among the data points.
2) For each data point x not chosen yet, **D(x)** is calculated, the distance between **x** and the nearest center that has already been chosen.
3) One new data point at random is chosen as a new center, using a weighted probability distribution where a point **x** is chosen with probability proportional to **D(x)^2**.
4) Steps 2 & 3 are repeated until K centers have been chosen.
5) Now that the initial centers have been chosen, we can proceed with the standard k-means clustering algorithm.

## Settings
![image](https://user-images.githubusercontent.com/79049601/212034530-0ae92650-ac51-401c-aaa5-4a4fee6898d1.png)
- K Value : the value of **K**.
- Bounding Box Size (XYZ) : the **dimensions** of a cube that limits the zone containing the clusters.
- Minimum & maximum number of points around centroid : the minimum and maximum bounds used to generate the **number of data points** around the initial centroids.
- Maximum distance from datapoints to centroid : the maximum **distance** used to describe how far the data points will be from their corresponding initial centroid. **The lower the value, the more visibly distinct the clusters will be.**
- K-Means++ : whether the **K-Means++** algorithm will be used or not.



## Under Development
- Better visualization of the SSE values over multiple iterations (graph).
- Allowing centroids to be moved and clusters to be updated during runtime.
- UI/UX Improvements.

## External Packages
- DOTween
