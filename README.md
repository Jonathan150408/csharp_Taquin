# Taquin in c#

> This repo contains the code of the famous game called _Taquin_. The goal of the game is to reorder the pieces in the grid using the empty square. You are allowed to "move" the empty square and exchange it with a number in the grid like this :
> <br>

<style>
    table {
        border-collapse: separate;
        border-spacing: 0;
    }
    td {
        /* couleur neutre par défaut pour éviter une bordure blanche qui écrase */
        border: 1px solid #ccc;
        padding: 4px;
    }
    .red {
        border: 1px solid red !important;
        /* fallback : forcer visuellement la bordure supérieure si nécessaire */
        box-shadow: 2px -1px red;
    }
</style>

## Before

<table>

  <tr>
    <td>1</td>
    <td>2</td>
    <td>3</td>
    <td>4</td>
  </tr>
  <tr>
    <td>5</td>
    <td>2</td>
    <td>7</td>
    <td>8</td>
  </tr> 
  <tr>
    <td>9</td>
    <td>10</td>
    <td>11</td>
    <td>12</td>
  </tr>
    <tr>
    <td class="red">Empty</td>
    <td class="red">13</td>
    <td>14</td>
    <td>15</td>
  </tr>
</table>

## After

<table>
  <tr>
    <td>1</td>
    <td>2</td>
    <td>3</td>
    <td>4</td>
  </tr>
  <tr>
    <td>5</td>
    <td>2</td>
    <td>7</td>
    <td>8</td>
  </tr> 
  <tr>
    <td>9</td>
    <td>10</td>
    <td>11</td>
    <td>12</td>
  </tr>
    <tr>
    <td class="red">13</td>
    <td class="red">Empty</td>
    <td>14</td>
    <td>15</td>
  </tr>
</table>

## Solved

<table>
  <tr>
    <td>1</td>
    <td>2</td>
    <td>3</td>
    <td>4</td>
  </tr>
  <tr>
    <td>5</td>
    <td>2</td>
    <td>7</td>
    <td>8</td>
  </tr> 
  <tr>
    <td>9</td>
    <td>10</td>
    <td>11</td>
    <td>12</td>
  </tr>
    <tr>
    <td>13</td>
    <td>14</td>
    <td>15</td>
    <td>Empty</td>
  </tr>
</table>
